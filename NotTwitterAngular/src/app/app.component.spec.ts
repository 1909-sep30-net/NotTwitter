import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { Component, NO_ERRORS_SCHEMA } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthService } from './auth.service';

describe('AppComponent', () => {
  beforeEach(async(() => {
    const authSpy = jasmine.createSpyObj('AuthService',
    ['localAuthSetup', 'handleAuthCallback']);

    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
      ],
      imports:[ 
        RouterTestingModule,
        HttpClientTestingModule
      ],
      
      schemas:[NO_ERRORS_SCHEMA]
    }).compileComponents();
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have false loading`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.loading).toBeFalsy;
  });

  it(`should have as title 'NotTwitterAngular'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual('NotTwitterAngular');
  });

});
