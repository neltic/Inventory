import { Component, inject } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { ICategory } from '../../../models/i-category';
import { CategoryService } from '../../../services/category-service';
import { TranslateDirective } from "../../directives/translate-directive";
import { FindInListPipe } from '../../pipes/find-in-list-pipe';
import { TranslateErrorPipe } from '../../pipes/translate-error-pipe';
import { BaseSelectComponent } from '../base-select/base-select';

@Component({
  selector: 'app-category-select',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
    FindInListPipe,
    TranslateErrorPipe,
    TranslateDirective
],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => inject(ControlContainer, { skipSelf: true })
    }
  ],
  templateUrl: './category-select.html',
  styleUrl: './category-select.scss',
})
export class CategorySelect extends BaseSelectComponent<ICategory> {
  public categoryService = inject(CategoryService);
  
  override initList() {
    this.catalogList = this.categoryService.getCategoriesByScope(this.scope);
  }
  
}
