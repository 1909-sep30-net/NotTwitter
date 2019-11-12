import { Component, OnInit } from '@angular/core';
import { NotTwitterAPIService } from '../../not-twitter-api.service';
import UserModel from '../../models/user-model';
import { AuthService } from 'src/app/auth.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})

export class UserComponent implements OnInit {
  users: UserModel[] = null;
  selectedUser: UserModel = null;
  visible:number = 1;
  showingProfile:boolean = false;
  name:string;
  
  getUserByName(name:string): void{
   this.NotTwitterApi.getUsersByName(name).then(users => this.users = users);
   this.visible = 1;
  }

  selectUser(user:UserModel):void{
    this.visible = 0;
    this.selectedUser = user;
    this.showingProfile = true;
  }
  getUserById(id:number): void{
    this.NotTwitterApi.getUsersById(id).then(user => this.selectedUser = user);
   }

  constructor(private NotTwitterApi: NotTwitterAPIService, public auth: AuthService) {}
  
  get user() {
    return this.NotTwitterApi.user;
  }

  ngOnInit() {

  }
  
}

