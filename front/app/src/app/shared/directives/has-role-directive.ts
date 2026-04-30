import { Directive, effect, inject, input, TemplateRef, ViewContainerRef } from '@angular/core';
import { Role } from '@models';
import { SecurityService } from '@services';

@Directive({
    selector: '[hasRole]',
    standalone: true
})
export class HasRoleDirective {
    private securityService = inject(SecurityService);
    private templateRef = inject(TemplateRef<any>);
    private viewContainer = inject(ViewContainerRef);

    role = input.required<Role>({ alias: 'hasRole' });

    constructor() {
        effect(() => {
            const roleToVerify = this.role();
            this.viewContainer.clear();
            if (this.securityService.hasRole(roleToVerify)) {
                this.viewContainer.createEmbeddedView(this.templateRef);
            }
        });
    }
}
