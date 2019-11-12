import { Component, OnInit } from '@angular/core';
import PostModel from 'src/app/models/post-model';
import { AuthService } from 'src/app/auth.service';
import { NotTwitterAPIService } from '../../not-twitter-api.service';

@Component({
  selector: 'app-list-posts',
  templateUrl: './list-posts.component.html',
  styleUrls: ['./list-posts.component.css']
})
export class ListPostsComponent implements OnInit {
  
  constructor(public auth: AuthService, private NotTwitterApi: NotTwitterAPIService) { }
  posts: PostModel = null;
  get user (){
    return this.NotTwitterApi.user;
  }

  ngOnInit() {
  }

}
