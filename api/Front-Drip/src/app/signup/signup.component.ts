import {Component, inject} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {environment} from "../../environments/environment.prod";

@Component({
  selector: 'signup',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: 'signup.component.html',
  styleUrls: ['signup.component.css']
})

export class SignupComponent{
  emailInput = new FormControl('', [Validators.required]);
  emailConfirmInput = new FormControl('', [Validators.required]);
  passwordInput = new FormControl('', [Validators.required]);
  passwordConfirmInput = new FormControl('', [Validators.required]);

  formGroup = new FormGroup({
    email: this.emailInput,
    emailConfirm: this.emailConfirmInput,
    password: this.passwordInput,
    passwordConfirm: this.passwordConfirmInput
  });

  clickSubmit(){
    if(this.verifyEmail() && this.verifyPassword()){
      console.log("wowow submit");
    }
    else{
      console.log("check that your email and password are correct");
    }
  }

  async signup(){
    if(this.verifyEmail() && this.verifyPassword()){
      const email = this.formGroup.get('email')?.value;
      const password = this.formGroup.get('password')?.value;

      const response = await fetch(environment.baseUrl + '/registeruser', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          email: email,
          password: password
        })
      });


      const isSuccess = await response.text();
      if(isSuccess === 'true'){
        window.location.href = '/login'
      }else{
        console.error('Something went wrong')
      }
    }
  }

  verifyPassword(){
    return this.passwordInput.value == this.passwordConfirmInput.value;
  }

  verifyEmail(){
    return this.emailInput.value == this.emailConfirmInput.value;
  }

}
