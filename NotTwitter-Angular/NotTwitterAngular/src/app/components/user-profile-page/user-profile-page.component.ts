import { Component, OnInit, Input } from '@angular/core';
import PostModel from '../../models/post-model';
import { NotTwitterAPIService } from '../../not-twitter-api.service';
import UserModel from '../../models/user-model';
import FriendModel from 'src/app/models/friend-model';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-user-profile-page',
  templateUrl: './user-profile-page.component.html',
  styleUrls: ['./user-profile-page.component.css']
})
export class UserProfilePageComponent implements OnInit {

  friends: FriendModel[];
  posts: PostModel[];

  loading: boolean = false;
  userSubscription:Subscription;
  loggedInUser: UserModel = this.notTwitApi.user;
  userOfProfile: UserModel;

  @Input() userId:number;

  constructor(
    private notTwitApi:NotTwitterAPIService
  ) { }


  ngOnInit() {
    this.loading = true;
    this.userSubscription = this.notTwitApi.userChanged.subscribe( newUser => 
      {
        this.loggedInUser = newUser;
        console.log("user loaded!!!! from user-profile");
        this.notTwitApi.getPostByUser(this.userId)
          .then(posts => {
            this.posts = posts;
            console.log(this.posts);
          });
      }
    );
    this.notTwitApi.getPostByUser(this.userId).then(posts => this.posts = posts);
    this.notTwitApi.getUsersById(this.userId).then(user => {this.userOfProfile = user;});

  }

  ngOnChanges(){
    this.notTwitApi.getPostByUser(this.userId).then(posts => this.posts = posts);
    this.notTwitApi.getUsersById(this.userId).then(user => {this.userOfProfile = user;});
  }

}
