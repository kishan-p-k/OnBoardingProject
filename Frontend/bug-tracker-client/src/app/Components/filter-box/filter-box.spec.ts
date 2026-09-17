import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FilterBox } from './filter-box';

describe('FilterBox', () => {
  let component: FilterBox;
  let fixture: ComponentFixture<FilterBox>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FilterBox],
    }).compileComponents();

    fixture = TestBed.createComponent(FilterBox);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
