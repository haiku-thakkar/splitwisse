import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllExpenseComponent } from './all-expense.component';

describe('AllExpenseComponent', () => {
  let component: AllExpenseComponent;
  let fixture: ComponentFixture<AllExpenseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllExpenseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllExpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
