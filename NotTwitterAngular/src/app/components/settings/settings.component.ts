import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { NotTwitterAPIService } from 'src/app/not-twitter-api.service';
import UserModel from 'src/app/models/user-model';
import UserModelUpdate from '../../models/user-model-update'
@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  updatedUser: UserModel;

  constructor(public auth: AuthService, private NotTwitterApi : NotTwitterAPIService) { }
  get user(){
    return this.NotTwitterApi.user;
  }
  //@Input() model:UserModel;

  UpdateUser(userId:number, updatedUser:UserModelUpdate) :void{
    this.NotTwitterApi.updateUser(userId,updatedUser).then(user=>this.updatedUser = user);
    console.log(updatedUser);
  }
  ngOnInit() {
  }

}
