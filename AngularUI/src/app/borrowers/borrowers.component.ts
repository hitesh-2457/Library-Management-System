import { Component, OnInit } from '@angular/core';
import { Borrowers } from '../models/borrowers';
import { QueryService } from '../services/query.service';

@Component({
  selector: 'app-borrowers',
  templateUrl: './borrowers.component.html',
  styleUrls: ['./borrowers.component.css']
})
export class BorrowersComponent implements OnInit {

  user: User;
  errors: string[];
  success: string;

  constructor(private query: QueryService) { }

  ngOnInit() {
    this.errors = [];
    this.user = new User();
    this.success = "";
  }

  onSubmit() {
    let borrower = new Borrowers();
    borrower.Bname = this.user.fname + "," + this.user.lname;
    borrower.Address = this.user.address + "," + this.user.city + "," + this.user.state;
    borrower.Email = this.user.emailAddress;
    borrower.Phone = this.user.phoneNumber;
    borrower.Ssn = this.user.ssn;

    this.query.createBorrower(borrower).subscribe(
      (data) => {
        if (data["error"]) {
          this.errors.push(data["error"]);
        }
        else {
          this.success = "User with ssn '" + borrower.Ssn + "' was created successfully.";
          this.user = new User();
        }
      },
      (error) => {
        this.errors.push("Failed to create the Borrower account. Please try again later.");
      }
    );
  }

  removeError(err: string) {
    this.errors.splice(this.errors.indexOf(err), 1);
  }
}

class User {
  fname: string;
  lname: string;
  ssn: number;
  phoneNumber: number;
  emailAddress: string;
  address: string;
  city: string;
  state: string;
}
