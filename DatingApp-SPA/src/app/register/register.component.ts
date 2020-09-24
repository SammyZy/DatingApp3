import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/Auth.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();

  model: any = {};

  constructor(private server: AuthService) { }

  ngOnInit() {
  }

  register(){
    // console.log(this.model);
    this.server.register(this.model).subscribe( () => {
      console.log('register success!');
    }, error => {
      console.log(error);
    });
  }

  cancel(){
    // console.log('cancelled');
    this.cancelRegister.emit(false);
  }

}
