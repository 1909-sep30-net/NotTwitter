import { Component, OnInit, Input } from '@angular/core';
import UserModel from 'src/app/models/user-model';
import { AuthService } from 'src/app/auth.service';
import { NotTwitterAPIService } from 'src/app/not-twitter-api.service';
import PostModel from 'src/app/models/post-model';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  user: UserModel;

  constructor(public auth: AuthService, private NotTwitterApi: NotTwitterAPIService) { }

  ngOnInit() {
    console.log(`this model: ${this.model.id}`);
    this.getUserInfo();
  }

  ngOnChanges(){
    console.log("loading user");
    this.user = this.NotTwitterApi.user;
  }

  @Input()model:UserModel;

  getUserInfo(): Promise<UserModel>{
    return this.NotTwitterApi.getUsersById(this.model.id).then(u=>this.user = u);
  }

}
