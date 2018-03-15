import { Component, OnInit, TemplateRef } from '@angular/core';
import { QueryService } from '../services/query.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Fines } from '../models/fines';
import { Borrowers } from '../models/borrowers';

@Component({
  selector: 'app-fines',
  templateUrl: './fines.component.html',
  styleUrls: ['./fines.component.css']
})
export class FinesComponent implements OnInit {

  loading: boolean;
  fines: any[];
  errors: string[];
  modalError: string;
  modalRef: BsModalRef;
  selectedRecord: any;

  constructor(private query: QueryService, private modalService: BsModalService) { }

  ngOnInit() {
    this.initialize();
  }

  initialize(){
    this.modalError = null;
    this.errors = [];
    this.fines = [];
    this.query.getFines().subscribe(
      (data) => {
        this.fines = data;
        console.log(this.fines);
      },
      (error) => {
        this.errors.push("Failed to fetch the fine data.");
      }
    );
  }

  removeError(err: string) {
    this.errors.splice(this.errors.indexOf(err), 1);
  }

  updateFine() {
    this.query.updateFines().subscribe(
      (data) => {
        this.fines = data;
        console.log(this.fines);
      },
      (error) => {
        this.errors.push("Failed to update the fines.");
      }
    );
  }

  showFine(template: TemplateRef<any>, fineRec: any) {
    this.selectedRecord = fineRec;
    this.modalError = null;
    this.modalRef = this.modalService.show(template);
  }

  payFine(fine: any) {
    this.query.payFine(fine.loanId, fine).subscribe(
      (data) => {
        this.modalRef.hide();
        this.initialize();
      },
      (error) => {
        this.modalError = "Failed to complete the payment. Try again later."
      }
    );
  }

  payAllFines(borrower: Borrowers) {
    this.query.payAllFines(borrower.CardId).subscribe(
      (data) => {
        this.modalRef.hide();
        this.fines = data;
      },
      (error) => {
        this.modalError = "Failed to complete the payment. Try again later."
      }
    );
  }

  totFine(list: any[]) {
    let total = 0;
    list.forEach(x => total += x.fine.fineAmt);
    return total;
  }
}
