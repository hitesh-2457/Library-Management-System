import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import { environment } from '../../environments/environment.prod';
import { HttpInterceptor } from '../Helpers/http.interceptor';
import { BookLoan } from '../models/book-loan';
import { Borrowers } from '../models/borrowers';
import { Fines } from '../models/fines';

@Injectable()
export class QueryService {

  private baseUrl = environment.baseUrl;
  constructor(private http: HttpInterceptor) { }

  searchBooks(search: string) {
    return this.http.get(this.baseUrl + "books/search/" + search).map(res => res.json());
  }

  getCheckedOuts() {
    return this.http.get(this.baseUrl + "bookLoan").map(res => res.json());
  }

  searchCheckOuts(search: string) {
    return this.http.get(this.baseUrl + "bookLoan/search/" + search).map(res => res.json());
  }

  checkOut(bookLoan: BookLoan) {
    return this.http.post(this.baseUrl + "bookLoan", JSON.stringify(bookLoan)).map(res => res.json());
  }

  checkIn(loan: BookLoan) {
    return this.http.put(this.baseUrl + "bookLoan/checkin/" + loan.LoanId, JSON.stringify(loan)).map(res => res.status === 200);
  }

  createBorrower(borrower: Borrowers) {
    return this.http.post(this.baseUrl + "borrower", JSON.stringify(borrower)).map(res => res.json());
  }

  getFines() {
    return this.http.get(this.baseUrl + "Fine").map(res => res.json());
  }

  updateFines() {
    return this.http.get(this.baseUrl + "Fine/update").map(res => res.json());
  }

  payFine(loanId: number, fine: Fines) {
    return this.http.put(this.baseUrl + "Fine/" + loanId, JSON.stringify(fine)).map(res => res.json());
  }

  payAllFines(cardId: string) {
    return this.http.get(this.baseUrl + "Fine/updateAll/" + cardId).map(res => res.json());
  }
}
