import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { DatePipe } from '@angular/common';
import { Component, computed, DestroyRef, inject, OnInit, signal, ViewChild } from '@angular/core';
import { rxResource, takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AbstractControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatOption, provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormField, MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatDivider, MatList, MatListItem, MatListModule } from '@angular/material/list';
import { MatSelect, MatSelectTrigger } from '@angular/material/select';
import { MatStep, MatStepper, MatStepperModule } from '@angular/material/stepper';
import { BoxStep, DetailsStep, IBoxLookup, IItem, IStorage, ItemStep } from '@models';
import { BoxService, BrandService, CategoryService, ItemService, StorageService } from '@services';
import { BaseFormComponent } from '../../shared/components/base-form/base-form';
import { BrandSelect } from '../../shared/components/brand-select/brand-select';
import { TranslateDirective } from "../../shared/directives/translate-directive";
import { TranslateErrorDirective } from "../../shared/directives/translate-error-directive";
import { AsPhotoPipe } from '../../shared/pipes/as-photo-pipe';

@Component({
    selector: 'app-storage',
    standalone: true,
    imports: [
        MatStepperModule,
        MatStep,
        MatCardModule,
        MatButtonModule,
        MatFormField,
        MatFormFieldModule,
        MatCheckboxModule,
        MatLabel,
        MatIcon,
        MatIconModule,
        MatInputModule,
        MatDatepickerModule,
        MatListModule,
        MatList,
        MatListItem,
        MatDivider,
        ReactiveFormsModule,
        MatSelect,
        MatSelectTrigger,
        MatOption,
        AsPhotoPipe,
        BrandSelect,
        DatePipe,
        TranslateDirective,
        TranslateErrorDirective
    ],
    providers: [
        provideNativeDateAdapter(),
        {
            provide: STEPPER_GLOBAL_OPTIONS,
            useValue: { displayDefaultIndicatorType: false },
        },
        { provide: BaseFormComponent, useExisting: Storage }
    ],
    templateUrl: './storage.html',
    styleUrl: './storage.scss',
})
export class Storage extends BaseFormComponent implements OnInit {
    public categoryService: CategoryService = inject(CategoryService);
    private boxService: BoxService = inject(BoxService);
    private itemService: ItemService = inject(ItemService);
    private brandService: BrandService = inject(BrandService);
    private storageService: StorageService = inject(StorageService);
    private destroyRef: DestroyRef = inject(DestroyRef);
    public filterText = signal<string>('');
    public filterCategory = signal<number | null>(null);
    public reviewSummary = signal<any>(null);
    public readonly minDate: Date = new Date();

    @ViewChild('stepper') stepper!: MatStepper;

    boxResourceList = rxResource<IBoxLookup[], any>({
        stream: () => this.boxService.getBoxesLookup()
    });

    itemResourceList = rxResource<IItem[], any>({
        stream: () => {
            return this.itemService.getItems()
        }
    });

    mainForm = this.fb.group({
        steps: this.fb.array([
            this.fb.group<BoxStep>({
                boxId: this.fb.control(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] })
            }),
            this.fb.group<ItemStep>({
                itemId: this.fb.control(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
                brandId: this.fb.control(-1, { nonNullable: true, validators: [Validators.required, this.brandService.validators.existsInScope(this.EntityScope.Item)] }),
            }),
            this.fb.group<DetailsStep>({
                quantity: this.fb.control(0, { nonNullable: true, validators: [Validators.min(0)] }),
                expires: this.fb.control(false, { nonNullable: true, validators: [Validators.required] }),
                expiresOn: this.fb.control(null),
                notes: this.fb.control(null)
            })
        ])
    });

    get steps() {
        return this.mainForm.controls.steps;
    }

    getStep(index: number): AbstractControl {
        return this.steps.at(index);
    }

    constructor() {
        super();
    }

    ngOnInit() {
        this.initComponent(['boxId', 'itemId', 'brandId', 'quantity', 'expires', 'expiresOn', 'notes']);

        this.getStep(2).get('expires')?.valueChanges
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe(expires => { this.toggleExpirationValidator(expires); });

        this.getStep(0).get('boxId')?.valueChanges
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe(val => { if (val > 0) setTimeout(() => this.stepper.next(), 250); });

        this.getStep(1).valueChanges
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe(values => {
                if (values.itemId > 0 && values.brandId !== -1) {
                    if (this.stepper.selectedIndex === 1) {
                        this.loadStorageData(() => {
                            setTimeout(() => this.stepper.next(), 250);
                        });
                    }
                }
            });
    }

    private toggleExpirationValidator(expires: boolean) {
        const control = this.getStep(2).get('expiresOn');
        if (expires) {
            control?.setValidators([Validators.required]);
        } else {
            control?.clearValidators();
            control?.setValue(null);
        }
        control?.updateValueAndValidity();
        this.updateErrorMessage('expiresOn', 'Storage.EXPIRATION_DATE');
    }

    private loadStorageData(callback: () => void) {
        const detailsStep = this.getStep(2);
        if (detailsStep.pristine) {
            const selectedData = this.getData();
            this.storageService.getStorage(selectedData.boxId, selectedData.itemId, selectedData.brandId).subscribe({
                next: (data: IStorage) => {
                    detailsStep.patchValue({
                        quantity: data.quantity,
                        expires: data.expires,
                        expiresOn: data.expiresOn,
                        notes: data.notes
                    }, { emitEvent: true });
                    callback();
                },
                error: (err) => { this.handleError(err); callback(); }
            });
        } else {
            callback();
        }
    }

    filteredList = computed(() => {
        const text = this.filterText().toLowerCase();
        const categoryId = this.filterCategory();
        const allItems = this.itemResourceList.value() ?? [];
        return allItems.filter(item => {
            const matchesText = item.name.toLowerCase().includes(text);
            const matchesCategory = categoryId === null || item.categoryId === categoryId;
            return matchesText && matchesCategory;
        });
    });

    onCategoryChange(id: number | null) {
        this.filterCategory.set(id);
    }

    onSearch(event: Event) {
        const val = (event.target as HTMLInputElement).value;
        this.filterText.set(val);
    }

    clearSearch() {
        this.filterText.set('');
    }

    save() {
        if (!this.mainForm.valid) return;

        const data = this.getData();
        const storage: IStorage = {
            ...data
        }

        this.storageService.saveStorage(storage)
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
                next: (response) => {
                    if (response.updated) {
                        this.openSnack('success', 'Global.OK', 'Message.STORAGE_UPDATED');
                        this.mainForm.reset();
                        this.mainForm.markAsPristine();
                        this.stepper.reset();
                        this.reviewSummary.set(null);
                    } else {
                        this.openSnack('error', 'Global.OK', 'Error.STORAGE_UPDATE_ERROR');
                    }
                },
                error: (error) => this.handleError(error)
            });
    }

    getData(): any {
        const [box, item, details] = this.steps.getRawValue();
        const data = {
            ...box,
            ...item,
            ...details
        };

        const formattedDate = data['expires'] && data['expiresOn']
            ? new Date(data['expiresOn']).toISOString().split('T')[0]
            : null;
        const finalData = {
            ...data,
            boxId: Array.isArray(data['boxId']) ? data['boxId'][0] : data['boxId'],
            itemId: Array.isArray(data['itemId']) ? data['itemId'][0] : data['itemId'],
            expiresOn: formattedDate
        };
        console.log(finalData);
        return finalData;
    }

    onStepChange(event: any) {

        if (event.selectedIndex < 2) {
            const detailsStep = this.getStep(2);
            detailsStep.markAsPristine();
            detailsStep.markAsUntouched();
        }

        if (event.selectedIndex === 3) {
            this.updateReviewSummary();
        }
    }

    private updateReviewSummary() {
        const data = this.getData();
        const boxes = this.boxResourceList.value() ?? [];
        const items = this.itemResourceList.value() ?? [];
        const brands = this.brandService.brands();
        const selectedBox = boxes.find(b => b.boxId === data.boxId);
        const selectedItem = items.find(i => i.itemId === data.itemId);
        const selectedBrand = brands.find(i => i.brandId === data.brandId);

        this.reviewSummary.set({
            boxName: selectedBox?.name ?? '---',
            itemName: selectedItem?.name ?? '---',
            brandName: selectedBrand?.name ?? '---',
            quantity: data.quantity,
            expires: data.expires,
            expiresOn: data.expiresOn,
            notes: data.notes
        });
    }

}
