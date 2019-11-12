import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from './auth.service';
import { NotTwitterAPIService } from './not-twitter-api.service';
import UserModel from './models/user-model';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  
  userSubscription:Subscription;
  loggedInUser: UserModel = this.notTwit.user;
  loading: boolean = false;
  title:string = "NotTwitterAngular";
  selectedUserId: number; // userid thats for displaying a selected user profile

  constructor(private auth: AuthService, private notTwit: NotTwitterAPIService) {
  }

  ngOnInit() {
    this.auth.localAuthSetup();
    this.auth.handleAuthCallback();
    this.userSubscription = this.notTwit.userChanged.subscribe( newUser => 
      {
        this.loggedInUser = newUser;
        console.log(`user loaded!!!! from app component ${this.loggedInUser.firstName}`);
        this.selectedUserId = this.loggedInUser.id;
      }
    );
    //console.log(this.notTwit.user);
  }

  getLoginUser(){
    this.loading = true;
    this.notTwit.user$.subscribe( data => {
      this.loading = false; // loading is done once the subscription is done
      this.loggedInUser = data; // populate the loggedinuser with the user data
    })
  }

}
