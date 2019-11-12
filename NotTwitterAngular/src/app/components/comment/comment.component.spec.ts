import { async, ComponentFixture, TestBed, getTestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CommentComponent } from './comment.component';
import { HttpClient } from 'selenium-webdriver/http';
import { NotTwitterAPIService } from 'src/app/not-twitter-api.service';

describe('CommentComponent', () => {
  let component: CommentComponent;
  let fixture: ComponentFixture<CommentComponent>;
  let service: NotTwitterAPIService;
  let injector: TestBed;
  let httpMock: HttpTestingController;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommentComponent, HttpClient ],
      imports: [HttpClientTestingModule],
      providers: [NotTwitterAPIService]
    })
    .compileComponents();
    injector = getTestBed();
    service = injector.get(NotTwitterAPIService);
    httpMock = injector.get(HttpTestingController);
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(()=>{
    httpMock.verify();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
