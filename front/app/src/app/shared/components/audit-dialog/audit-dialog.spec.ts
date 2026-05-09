import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditDialog } from './audit-dialog';

describe('AuditDialog', () => {
  let component: AuditDialog;
  let fixture: ComponentFixture<AuditDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuditDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuditDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
