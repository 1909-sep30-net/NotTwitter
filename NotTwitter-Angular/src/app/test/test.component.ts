import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {

  constructor() { }
 
  ngOnInit() {
  }
  forgot=false
  toggleEvent(){
    this.forgot = !this.forgot
  }

}
