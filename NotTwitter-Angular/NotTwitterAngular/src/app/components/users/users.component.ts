import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {NotTwitterAPIService} from '../../not-twitter-api.service';
@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  user:any;

  constructor(public rest:NotTwitterAPIService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {

  }
}
