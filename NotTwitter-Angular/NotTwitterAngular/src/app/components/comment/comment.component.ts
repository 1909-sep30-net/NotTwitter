import { Component, OnInit, Input } from '@angular/core';
import { NotTwitterAPIService } from 'src/app/not-twitter-api.service';
import CommentModel from 'src/app/models/comment-model';
import UserModel from 'src/app/models/user-model';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {

  author:UserModel=null;
  loaded:boolean=false;

  loadComment():void{
    this.NotTwitterService.getUsersById(this.model.authorId).then(user=>this.author = user);
    this.loaded = true;
  }

  @Input() model: CommentModel;

  constructor(
    private NotTwitterService: NotTwitterAPIService
  ) { }

  ngOnInit() {
    this.loadComment();
    console.log("comment loaded?");
  }


}
