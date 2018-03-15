import { Component, OnInit, TemplateRef } from '@angular/core';
import { QueryService } from '../services/query.service';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap';
import { BsModalRef } from 'ngx-bootstrap';
import { BookLoan } from '../models/book-loan';
import { Fines } from '../models/fines';

@Component({
  selector: 'app-checkin',
  templateUrl: './checkin.component.html',
  styleUrls: ['./checkin.component.css']
})
export class CheckinComponent implements OnInit {

  fine: Fines;
  checkedIn: Boolean;
  modalRef: BsModalRef;
  modalError: string;
  checkOuts: any[];
  errors: string[];
  search: string;
  loading: Boolean;
  loan: BookLoan;
  selectedRecord: any;

  constructor(private query: QueryService, private modalService: BsModalService) { }

  ngOnInit() {
    this.loading = false;
    this.errors = [];
    this.checkOuts = [];
    this.query.getCheckedOuts().subscribe(
      (data) => {
        this.checkOuts = data;
        console.log(this.checkOuts);
      },
      (error) => {
        this.errors.push("Failed to fetch all the checked out books. Try again later!!!");
      }
    );
  }

  onSubmit() {
    this.loading = true;
    this.query.searchCheckOuts(this.search).subscribe(
      (data) => {
        this.checkOuts = data;
        this.loading = false;
        console.log(this.checkOuts);
      },
      (error) => {
        this.loading = false;
        this.errors.push("Failed to fetch all the checked out books. Try again later!!!");
      }
    );
  }

  removeError(err: string) {
    this.errors.splice(this.errors.indexOf(err), 1);
  }

  checkInModel(template: TemplateRef<any>, selectedRecord: any) {
    this.fine = selectedRecord['bookLoan']['Fines'];
    this.loan = selectedRecord['bookLoan'];
    this.selectedRecord = selectedRecord;
    this.checkedIn = false;
    this.checkInBook(template);
  }

  checkInBook(template: TemplateRef<any>) {
    this.query.checkIn(this.loan).subscribe(
      (data) => {
        this.checkOuts[this.checkOuts.indexOf(this.selectedRecord)]["book"]["IsCheckedOut"] = false;
        this.checkedIn = true;
        if (this.fine != null && this.checkedIn) {
          this.modalError = null;
          this.modalRef = this.modalService.show(template);
        }
      },
      (error) => {
        this.errors.push("Failed to check in the book. Please try again later.");
      }
    );
  }

  finePaid() {
    this.query.payFine(this.fine.LoanId, this.fine).subscribe(
      (data) => {
        this.modalRef.hide();
        this.fine.FineAmt = 0;
      },
      (error) => {
        this.modalError = "Failed to complete the payment. Try again later."
      }
    );
  }
}
