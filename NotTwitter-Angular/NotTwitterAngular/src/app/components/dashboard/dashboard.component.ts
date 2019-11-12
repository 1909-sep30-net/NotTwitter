import { Component, OnInit, Input } from '@angular/core';
import PostModel from '../../models/post-model';
import { NotTwitterAPIService } from '../../not-twitter-api.service';
import UserModel from '../../models/user-model';
import FriendModel from 'src/app/models/friend-model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  friends: FriendModel[];
  posts: PostModel[];

  loading: boolean = false;
  userSubscription:Subscription;
  loggedInUser: UserModel = this.notTwitApi.user;

  constructor(
    private notTwitApi:NotTwitterAPIService
  ) { }

  ngOnInit() {
    this.loading = true;
    this.userSubscription = this.notTwitApi.userChanged.subscribe( newUser => 
      {
        this.loggedInUser = newUser;
        //console.log("user loaded!!!! from dashboard");
        //this.notTwitApi.getUsersById(newUser.id).then(u => {this.friends = u.friends}).then(()=>{console.log("friends loaded - dashboard")});
        this.notTwitApi.getFriendPosts(this.loggedInUser.id)
          .then(posts => {
            this.posts = posts;
            console.log(this.posts);
          })
          .then(()=>{this.loading=false;});
        //this.getLoginUser();
      }
    );
    console.log("dashboard re initted");
    if (this.posts == null && this.loggedInUser != null){
      this.notTwitApi.getFriendPosts(this.loggedInUser.id)
          .then(posts => {
            this.posts = posts;
            console.log(this.posts);
          })
          .then(()=>{this.loading=false;});
    }

  }

  ngOnDestroy() {
    //prevent memory leak when component destroyed
     this.userSubscription.unsubscribe();
   }

}
