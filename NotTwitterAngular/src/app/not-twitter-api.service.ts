import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import UserModel from './models/user-model';
import PostModel from './models/post-model';
import UserModelCreate from './models/user-model-create';
import UserModelUpdate from './models/user-model-update';

import CommentModel from './models/comment-model';

import FriendRequestModel from './models/friendrequest-model';
import { AuthService } from './auth.service';
import CommentCreate from './models/comment-create-model';
import { Observable, from, Subject } from 'rxjs';
import { map, mergeMap } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})

/*
If PUT/POST requests not workinh as inteded, might have to create models with specific values for each of the controller calls
*/
export class NotTwitterAPIService {
  user$: Observable<UserModel> = null;
  user: UserModel;
  userChanged = new Subject<UserModel>();
  // constructor(
  //   private httpClient: HttpClient,
  //   auth: AuthService
  // ) {
  //   this.user$ = auth.userProfile$.pipe(mergeMap(data => {
  //     if (!data) {
  //       console.log("data was null");
  //       return null;
  //     } else {
  //       console.log("data not null");
  //       return from(this.getUserByEmail(data.email)
  //         .catch((err: HttpErrorResponse) => {
  //           if (err.status === 404) {
  //             // user does not exist, create
  //             throw err;
  //           } else {
  //             throw err;
  //           }
  //         }
  //       ));
  //     }
  //   }));
  // }

  constructor(
    private httpClient: HttpClient,
    auth: AuthService
  ) {
    auth.userProfile$.subscribe(user => {
      if (user) {
        this.getUserByEmail(user.email).catch((err: HttpErrorResponse) => {
          if (err.status === 404) {
            // if user does not exist, create
            //return this.createUser({ email: user.email, name: user.name });
            throw err;
          } else {
            throw err;
          }
        }).then(apiUser => {
          this.user = apiUser;
          this.userChanged.next(this.user);
        });
      }
    });

  }

  // constructor(
  //   private httpClient: HttpClient,
  //   auth: AuthService
  // ) {
  //   auth.userProfile$.subscribe(user => {
  //     if (user) {
  //       this.getUserByEmail(user.email).catch((err: HttpErrorResponse) => {
  //         if (err.status === 404) {
  //           // if user does not exist, create
  //           //return this.createUser({ email: user.email, name: user.name });
  //           throw err;
  //         } else {
  //           throw err;
  //         }
  //       }).then(apiUser => {
  //         this.user = apiUser;
  //       });
  //     }
  //   });
  // }



  /*
  User Controller Functionality
  */
  getUsersByName(name: string): Promise<UserModel[]> {
    const url = `${environment.notTwitterApiBaseUrl}/api/User/name/${name}`;
    return this.httpClient.get<UserModel[]>(url).toPromise();
  }

  getUserByEmail(email: string): Promise<UserModel> {
    const url = `${environment.notTwitterApiBaseUrl}/api/user/email/${email}`;
    return this.httpClient.get<UserModel>(url).toPromise();
  }

  getUsersById(id: number): Promise<UserModel> {
    const url = `${environment.notTwitterApiBaseUrl}/api/User/${id}`;
    return this.httpClient.get<UserModel>(url).toPromise();
  }

  getFriendPosts(id: number): Promise<PostModel[]> {
    const url = `${environment.notTwitterApiBaseUrl}/api/User/friendposts/${id}`;
    return this.httpClient.get<PostModel[]>(url).toPromise();
  }

  createUser(id: number, newUser: UserModelCreate) {
    const url = `${environment.notTwitterApiBaseUrl}/api/User`;
    return this.httpClient.post<UserModel>(url, { id, newUser }).toPromise();
  }

  updateUser(id: number, updatedUser: UserModelUpdate) {
    const url = `${environment.notTwitterApiBaseUrl}/api/User/${id}`;
    return this.httpClient.put<UserModel>(url, { id, updatedUser }).toPromise();
  }

  deleteUser(id: number) {
    const url = `${environment.notTwitterApiBaseUrl}/api/User/${id}`;
    return this.httpClient.delete(url).toPromise();
  }


  /*
  Post Controller Functionality
  */

  getPostById(postId: number): Promise<PostModel> {
    const url = `${environment.notTwitterApiBaseUrl}/api/Post/${postId}`;
    return this.httpClient.get<PostModel>(url).toPromise();
  }

  getPostByUser(userId: number): Promise<PostModel[]> {
    const url = `${environment.notTwitterApiBaseUrl}/api/Post/user/${userId}`;
    return this.httpClient.get<PostModel[]>(url).toPromise();
  }

  createPost(authorId: number, content: string) {
    const url = `${environment.notTwitterApiBaseUrl}/api/Post`;
    //trying adding body of post request as {id, content} if not working, try making a new model PostModelCreate that
    //that exclusively id and content as fields.
    return this.httpClient.post<PostModel>(url, { authorId, content }).toPromise();
  }

  updatePost(postId: number, postModel: PostModel) {
    const url = `${environment.notTwitterApiBaseUrl}/api/Post/${postId}`;
    return this.httpClient.put<PostModel>(url, { postId, postModel }).toPromise();
  }

  deletePost(postId: number) {
    const url = `${environment.notTwitterApiBaseUrl}/api/Post/${postId}}`;
    return this.httpClient.delete(url).toPromise();
  }


  /*
  Comment Controller Functionality
  */

  getCommentById(commentId: number): Promise<CommentModel> {
    const url = `${environment.notTwitterApiBaseUrl}/api/Comment/${commentId}`;
    return this.httpClient.get<CommentModel>(url).toPromise();
  }

  getCommentByUserId(userId: number): Promise<CommentModel> {
    const url = `${environment.notTwitterApiBaseUrl}/api/Comment/user/${userId}`;
    return this.httpClient.get<CommentModel>(url).toPromise();
  }

  createComment(commentModel: CommentCreate) {
    const url = `${environment.notTwitterApiBaseUrl}/api/Comment/Post`;
    return this.httpClient.post<CommentCreate>(url, commentModel).toPromise();
  }

  updateComment(commentId: number, commentModel: CommentModel) {
    const url = `${environment.notTwitterApiBaseUrl}/api/Comment/${commentId}`;
    return this.httpClient.put<CommentModel>(url, { commentId, commentModel }).toPromise();
  }

  //API parameters for delete comment seem to require both postId and a postModel. However httpclient.delete only allows for url and options. 
  //Further inspection of delete in comment API it seems that postModel parameter doesnt really get used, so only using postId for deleteComment(postId)
  //might work as intended
  deleteComment(postId: number) {
    const url = `${environment.notTwitterApiBaseUrl}/api/Comment/${postId}`;
    return this.httpClient.delete(url).toPromise();
  }

  /*
  FriendRequest controller
  */

  getFriendRequest(userId: number): Promise<FriendRequestModel[]> {
    const url = `${environment.notTwitterApiBaseUrl}/api/FriendRequest/${userId}`;
    return this.httpClient.get<FriendRequestModel[]>(url).toPromise();
  }

  createRequest(friendRequest: FriendRequestModel) {
    const url = `${environment.notTwitterApiBaseUrl}/api/FriendRequest/Create`;
    return this.httpClient.post<PostModel>(url, friendRequest).toPromise();
  }

  acceptRequest(friendRequest: FriendRequestModel) {
    const url = `${environment.notTwitterApiBaseUrl}/api/FriendRequest/Accepted`;
    return this.httpClient.patch<FriendRequestModel>(url, friendRequest).toPromise();
  }

  declineRequest(friendRequest: FriendRequestModel) {
    const url = `${environment.notTwitterApiBaseUrl}/api/FriendRequest/Declined`;
    return this.httpClient.post<PostModel>(url, friendRequest).toPromise();
  }

}


