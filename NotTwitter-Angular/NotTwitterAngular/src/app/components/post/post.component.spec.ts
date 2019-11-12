import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NotTwitterAPIService } from '../../not-twitter-api.service';
import { PostComponent } from './post.component';
import { CommentComponent } from '../comment/comment.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';


describe('PostComponent', () => {
  let component: PostComponent;
  let fixture: ComponentFixture<PostComponent>;

  beforeEach(async(() => {
    const twitSpy = jasmine.createSpyObj('NotTwitterAPIService',['getUsersByName']);
    // get users by name can return an empty array
    twitSpy.getUsersByName.and.returnValue(Promise.resolve([]));

    TestBed.configureTestingModule({
      declarations: [ PostComponent, CommentComponent ],
      imports:[ReactiveFormsModule],
      schemas:[CUSTOM_ELEMENTS_SCHEMA],
      providers: [
        { provide: NotTwitterAPIService, useValue: twitSpy}
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () =>{
    expect(component).toBeTruthy();
  })

  it('should create NotTwitService', () => {
    const service: NotTwitterAPIService = TestBed.get(NotTwitterAPIService);
    expect(service).toBeTruthy();
  });

  it(`should have false loading`, () => {
    const fixture = TestBed.createComponent(PostComponent);
    const comp = fixture.debugElement.componentInstance;
    expect(comp.loggedInUser).toBeNull;
  });
});
