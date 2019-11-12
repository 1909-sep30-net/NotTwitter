import { Component, OnInit, Input } from '@angular/core';
import CommentCreate from 'src/app/models/comment-create-model';
import CommentModel from 'src/app/models/comment-model';

import PostModel from 'src/app/models/post-model'
import { NotTwitterAPIService } from 'src/app/not-twitter-api.service';
import UserModel from 'src/app/models/user-model';
import { Subscription } from 'rxjs';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {

  loggedInUser:UserModel = null;
  currentUser:UserModel = null;
  myPostId:number = null;
  userSubscription:Subscription;
  content = new FormControl('');
  maxNumOfComments: number = 3;
  
  constructor(private notTwitApi:NotTwitterAPIService) { 
    this.loggedInUser = null;
    this.currentUser = null;
    this.maxNumOfComments = 3;

  }

  @Input() model:PostModel;
  @Input() postId:number;

  ngOnInit() {
    //console.log(`ngOnInit fired, post id is ${this.postId}`);
    //console.log(`model post id: ${this.model.postID}`);
    //console.log(`current post author: ${this.notTwitApi.user.id}`);
    this.userSubscription = this.notTwitApi.userChanged.subscribe( newUser => 
      {
        this.loggedInUser = newUser;
        //console.log(`user loaded!!!! from post, it's id is: ${this.loggedInUser.id}`);
        //this.getLoginUser();
        this.loadPost();
      }
    );
    if (this.loggedInUser != null){
      this.loadPost();
    }
    //this.loadPost();
  }

  ngOnChanges(){
    //console.log(`incoming post id: ${this.model.postID}`);
    this.loadPost();
  }


  loadPost():void{
    this.notTwitApi.getPostById(this.postId).then(newPost=>this.model = newPost);

  }

  extendList(extendBy:number){
    this.maxNumOfComments += extendBy;
  }

  addComment(content:string):void{
    let newComment:CommentCreate = {
      content: content,
      postId: this.postId,
      authorId: this.notTwitApi.user.id
    }
    this.notTwitApi.createComment(newComment);
    console.log(`added comment by ${this.notTwitApi.user.username}`);
    console.log(`for postid: ${this.myPostId}`);
    this.loadPost();
    let mockComment:CommentModel = {
      content: content,
      postId: this.postId,
      authorId: this.notTwitApi.user.id,
      commentId: 0,
      timeSent: new Date(Date.now())
    };
    this.model.comments.push(mockComment);

  }



}
