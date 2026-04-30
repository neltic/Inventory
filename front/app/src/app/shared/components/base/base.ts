import { Location } from '@angular/common';
import { Directive, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarRef, TextOnlySnackBar } from '@angular/material/snack-bar';
import { GlobalizationKey } from '@core';
import { EntityScope, Role, SCOPE_NONE_OPTION, SCOPE_OPTIONS, SCOPE_SELECTABLE_OPTIONS, ScopeOption } from '@models';
import { GlobalizationService, RouteParamsService, SecurityService } from '@services';
import { firstValueFrom } from 'rxjs';
import { ConfirmDialog } from '../confirm-dialog/confirm-dialog';

@Directive()
export abstract class BaseComponent {

    private snackBar: MatSnackBar = inject(MatSnackBar);
    private location: Location = inject(Location);
    protected readonly dialog: MatDialog = inject(MatDialog);
    protected params: RouteParamsService = inject(RouteParamsService);
    protected globalization: GlobalizationService = inject(GlobalizationService);
    protected readonly scopeOptions = SCOPE_OPTIONS;
    protected readonly scopeSelectableOptions = SCOPE_SELECTABLE_OPTIONS;
    public securityService: SecurityService = inject(SecurityService);
    public readonly EntityScope = EntityScope;
    public readonly Role = Role;

    openSnack(type: 'success' | 'error' | 'info' | 'warning', actionKey: GlobalizationKey, messageKey: GlobalizationKey | string): MatSnackBarRef<TextOnlySnackBar>;
    openSnack(type: 'success' | 'error' | 'info' | 'warning', actionKey: GlobalizationKey, messageKey: GlobalizationKey | string, params: any[]): MatSnackBarRef<TextOnlySnackBar>;
    openSnack(type: 'success' | 'error' | 'info' | 'warning', actionKey: GlobalizationKey, messageKey: GlobalizationKey | string, params?: any[]): MatSnackBarRef<TextOnlySnackBar> {
        const translatedAction = this.globalization.translate(actionKey);
        const translatedMessage = this.globalization.translate(messageKey, params || []);
        return this.snackBar.open(translatedMessage, translatedAction, {
            duration: 3000,
            panelClass: [type + '-snackbar']
        });
    }

    async openConfirm(type: 'success' | 'error' | 'info' | 'warning', questionKey: GlobalizationKey, params: any[] = []): Promise<boolean> {
        const translatedQuestion = this.globalization.translate(questionKey, params);
        const dialogRef = this.dialog.open(ConfirmDialog, {
            data: { type: type, question: translatedQuestion },
        });
        const result = await firstValueFrom(dialogRef.afterClosed());
        return !!result;
    }
    async openSuccess(questionKey: GlobalizationKey, params: any[] = []): Promise<boolean> { return this.openConfirm('success', questionKey, params); }
    async openError(questionKey: GlobalizationKey, params: any[] = []): Promise<boolean> { return this.openConfirm('error', questionKey, params); }
    async openWarning(questionKey: GlobalizationKey, params: any[] = []): Promise<boolean> { return this.openConfirm('warning', questionKey, params); }
    async openInfo(questionKey: GlobalizationKey, params: any[] = []): Promise<boolean> { return this.openConfirm('info', questionKey, params); }

    goBack(): void {
        this.location.back();
    }

    handleError(error: any, customErrorMessage?: GlobalizationKey) {
        const apiMessage = error.error?.message;
        const msg = (error.status >= 400 && error.status <= 500 && apiMessage)
            ? apiMessage
            : (customErrorMessage || 'Error.EXECUTION_ERROR');

        this.openSnack('error', 'Global.OK', msg);

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
        toScope: (arr: number[]): EntityScope => {
            return (arr || []).reduce((acc, curr) => acc | curr, EntityScope.None);
        },
        getById: (id: number, type: 'all' | 'selectable' = 'all'): ScopeOption => {
            const scopeList = type === 'all' ? this.scopeOptions : this.scopeSelectableOptions;
            return scopeList.find(opt => opt.value === id) ?? SCOPE_NONE_OPTION;
        }
    };
}