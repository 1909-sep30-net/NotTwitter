import { TestBed } from '@angular/core/testing';

import { NotTwitterAPIService } from './not-twitter-api.service';
import { AuthService } from './auth.service';
import UserModel from './models/user-model'
import { Router } from '@angular/router';

describe('NotTwitterAPIService', () => {
  let service:NotTwitterAPIService;
  beforeEach(() => {
    TestBed.configureTestingModule({providers:[NotTwitterAPIService, AuthService]});});

  it('should be created', () => {
    const service: NotTwitterAPIService = TestBed.get(NotTwitterAPIService);
    expect(service).toBeTruthy();
  });

  it('user should be defined', () => {
    const service: NotTwitterAPIService = TestBed.get(NotTwitterAPIService)
    expect(service.user).toBeDefined();
  })
});
