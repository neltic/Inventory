import { Location } from '@angular/common';
import { Directive, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarRef, TextOnlySnackBar } from '@angular/material/snack-bar';
import { firstValueFrom } from 'rxjs';
import { EntityScope, SCOPE_NONE_OPTION, SCOPE_OPTIONS, SCOPE_SELECTABLE_OPTIONS, ScopeOption } from '../../../models/e-entity-scope';
import { RouteParamsService } from '../../../services/route-params-service';
import { ConfirmDialog } from '../confirm-dialog/confirm-dialog';

@Directive()
export abstract class BaseComponent {

    private snackBar = inject(MatSnackBar);
    private location = inject(Location); 
    protected readonly dialog = inject(MatDialog);
    protected params = inject(RouteParamsService);
    protected readonly scopeOptions = SCOPE_OPTIONS;
    protected readonly scopeSelectableOptions = SCOPE_SELECTABLE_OPTIONS;    
    public readonly EntityScope = EntityScope;    

    openSnack(type: 'success' | 'error' | 'info' | 'warning', message: string, actionName: string) : MatSnackBarRef<TextOnlySnackBar> {
        return this.snackBar.open(message, actionName, { 
            duration: 3000, 
            panelClass: [type + '-snackbar'] 
        });
    }

    async openConfirm(type: 'success' | 'error' | 'info' | 'warning', question: string): Promise<boolean> {
        const dialogRef = this.dialog.open(ConfirmDialog, {
            data: { type: type, question: question },
        });
        const result = await firstValueFrom(dialogRef.afterClosed());
        return !!result;
    }
    async openSuccess(question: string): Promise<boolean> { return this.openConfirm('success', question); }
    async openError(question: string): Promise<boolean> { return this.openConfirm('error', question); }
    async openWarning(question: string): Promise<boolean> { return this.openConfirm('warning', question); }
    async openInfo(question: string): Promise<boolean> { return this.openConfirm('info', question); }

    goBack() : void {
        this.location.back();
    }    

    handleError(error: any, customErrorMessage?: string) {
        const apiMessage = error.error?.message;
        const msg = (error.status >= 400 && error.status <= 500 && apiMessage)
                    ? apiMessage
                    : (customErrorMessage || 'Error saving data');
        
        this.openSnack('error', msg, 'Close');

        if (error.error?.detail) {
            console.error('Detail:', error.error.detail);
        }
    }

    isDarkColor(hex: string | undefined): boolean {
        if (!hex) return false;

        const color = hex.replace('#', '');
        
        const r = parseInt(color.substring(0, 2), 16);
        const g = parseInt(color.substring(2, 4), 16);
        const b = parseInt(color.substring(4, 6), 16);
        
        const luminance = (0.299 * r + 0.587 * g + 0.114 * b);
        
        return luminance < 64;
    }

    readonly entityScope = {
        toCompactScopeArray: (scope: number | EntityScope): ScopeOption[] => {
            const allSelectableValue = this.scopeSelectableOptions.reduce((acc, opt) => acc | opt.value, 0);
            const isFullScope = (scope & allSelectableValue) === allSelectableValue || scope === EntityScope.General;
            if (isFullScope) {
                return this.scopeOptions.filter(opt => opt.value === EntityScope.General);
            }
            return this.scopeSelectableOptions.filter(opt => (scope & opt.value) === opt.value);
        },
        toScopeArray: (scope: number | EntityScope): ScopeOption[] => {            
            return this.scopeOptions.filter(opt => (scope & opt.value) === opt.value);            
        },
        toArray: (scope: number | EntityScope, type: 'all' | 'selectable' = 'selectable'): EntityScope[] => {
            const scopeList = type === 'all' ? this.scopeOptions : this.scopeSelectableOptions;        
            let initialArray: EntityScope[] = [];
            if (scope === EntityScope.General) {
                initialArray = scopeList.map(opt => opt.value);
            } else {
                initialArray = scopeList.filter(opt => (scope & opt.value) === opt.value).map(opt => opt.value);
            }
            return initialArray;
        },
        toScope: (arr: number[]) : EntityScope => {
            return (arr || []).reduce((acc, curr) => acc | curr, EntityScope.None);    
        },
        getById: (id: number, type: 'all' | 'selectable' = 'all') : ScopeOption => {
            const scopeList = type === 'all' ? this.scopeOptions : this.scopeSelectableOptions;
            return scopeList.find(opt => opt.value === id) ?? SCOPE_NONE_OPTION;
        }
    };
}