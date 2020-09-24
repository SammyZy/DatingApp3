import { Component, OnInit, ɵConsole } from '@angular/core';
import { AuthService } from '../_services/Auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login(){
    // console.log(this.model);
    this.authService.login(this.model).subscribe(next => {
      console.log( 'success!');
    }, error => {
      console.log( 'error!');
    });
  }

  loggedIn(){
    const token = localStorage.getItem('token');
    // !! 如果明确设置了变量的值（非null/undifined/0/""/NaN等值),结果就会根据变量的实际值来返回，如果没有设置，结果就会返回false
    return !!token;
  }

  logout(){
    localStorage.removeItem('token');
    console.log('logged out');
  }

}
