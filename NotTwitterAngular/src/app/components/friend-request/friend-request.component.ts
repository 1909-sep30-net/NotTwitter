import { Component, OnInit, Input } from '@angular/core';
import { NotTwitterAPIService } from 'src/app/not-twitter-api.service';
import FriendRequestModel from 'src/app/models/friendrequest-model';
import UserModel from 'src/app/models/user-model';
import StatusNames from 'src/app/models/status-names';
import { Subscription } from 'rxjs';
import FriendModel from 'src/app/models/friend-model';

@Component({
  selector: 'app-friend-request',
  templateUrl: './friend-request.component.html',
  styleUrls: ['./friend-request.component.css']
})
export class FriendRequestComponent implements OnInit {
  //get the users friends, display their names, display friend requests and friend requests time sent , friend request status
  //go to profile, remove friend
  //it needs to handle accepting or declining requests.
  
  user : UserModel;
  userName: UserModel;
  requestUserModel: UserModel;
  loggedInUser : UserModel = null;
  userSubscription : Subscription;
  friends: FriendModel[] = null;

  statusNames: StatusNames [] = [];
  requestStatus: number [] = [];
  userFriendRequests: UserModel[] = [];
  
  id = this.NotTwitterAPI.user.id;
  request: FriendRequestModel = {senderId : 0, receiverId : 0, status: 0};
  friendRequests: FriendRequestModel [];

  loaded:boolean = false;

  //friendRequestStatus = ['Pending Response', 'You are now friends!', 'Declined'];


  getUserFriends(){
    console.log(`getting user friends from id: ${this.loggedInUser.id}`);
    this.NotTwitterAPI.getUsersById(38).then(user => this.user = user).then(()=>this.friends = this.user.friends)
    .then( ()=>{
      for (let friend of this.friends){
        console.log(friend);
      };
      this.loaded = true;
    });
    
  }

  getUserFriendRequests(){
    this.NotTwitterAPI.getFriendRequest(this.loggedInUser.id).then(friendRequest => this.friendRequests = friendRequest)
    .then( () => { 
      for (let entry of this.friendRequests) { 
        this.NotTwitterAPI.getUsersById(entry.senderId)
          .then(newUser => this.requestUserModel = newUser)
          .then( () => this.statusNames.push({name: this.requestUserModel.firstName + " " + this.requestUserModel.lastName, status: entry.status, id: entry.senderId}));
      } 
    }
    );
  }

  acceptFriendRequest(senderId:number){
    this.request.receiverId = this.NotTwitterAPI.user.id;
    this.request.senderId = senderId;
    this.NotTwitterAPI.acceptRequest(this.request);
  }

  declineFriendRequest(senderId: number){
    this.request.receiverId = this.NotTwitterAPI.user.id;
    this.request.senderId = senderId;
    this.NotTwitterAPI.declineRequest(this.request);
  }
  
  constructor(private NotTwitterAPI: NotTwitterAPIService) { 
    this.loaded = false;
  }

  ngOnInit() {
    this.loaded = false;
    this.loggedInUser = this.NotTwitterAPI.user;
    console.log(`loaded on init?: ${this.loaded}`);
    // this.getUserFriendRequests();
    // this.getUserFriends();

    console.log(`loggedinuser currently: ${this.loggedInUser}`);
    this.userSubscription = this.NotTwitterAPI.userChanged.subscribe( newUser => 
      {
        console.log("user loaded from friendrequest");
        this.loggedInUser = newUser;
        this.getUserFriendRequests();
        this.getUserFriends();
      }
    );
    if (this.loggedInUser != null){
      this.getUserFriendRequests();
      this.getUserFriends();
    }

  }
  checkLoad(){
    console.log(this.loggedInUser.username);
    //console.log(`or is user friend populated: ${this.friends[0].FirstName}`)
  }
}