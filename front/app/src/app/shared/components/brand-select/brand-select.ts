import { Component, inject } from '@angular/core';
import { ControlContainer, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { IBrand } from '../../../models/i-brand';
import { BrandService } from '../../../services/brand-service';
import { FindInListPipe } from '../../pipes/find-in-list-pipe';
import { BaseSelectComponent } from '../base-select/base-select';

@Component({
  selector: 'app-brand-select',
  imports: [
    ReactiveFormsModule, 
    MatFormFieldModule,
    MatSelectModule, 
    MatIconModule,    
    FindInListPipe
  ],
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => inject(ControlContainer, { skipSelf: true })
    }
  ],
  templateUrl: './brand-select.html',
  styleUrl: './brand-select.scss',
})
export class BrandSelect extends BaseSelectComponent<IBrand> {
  public brandService = inject(BrandService);
  
  override initList() {
    this.catalogList = this.brandService.getBrandsByScope(this.scope);
  }

}