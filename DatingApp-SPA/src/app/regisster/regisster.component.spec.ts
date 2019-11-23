/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RegissterComponent } from './regisster.component';

describe('RegissterComponent', () => {
  let component: RegissterComponent;
  let fixture: ComponentFixture<RegissterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegissterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegissterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
