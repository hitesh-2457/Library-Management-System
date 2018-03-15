import { Component, OnInit, TemplateRef } from '@angular/core';
import { QueryService } from '../services/query.service';
import { Books } from '../models/books';
import { ModalDirective } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap';
import { BsModalRef } from 'ngx-bootstrap';
import { BookLoan } from '../models/book-loan';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  modalRef: BsModalRef;
  modalError: string;
  search: string;
  errors: string[];
  books: any[];
  loading: boolean;
  selectedBook: any;
  loan: BookLoan;
  btnStatus: Boolean;

  constructor(private query: QueryService, private modalService: BsModalService) { }

  ngOnInit() {
    this.books = [];
    this.errors = [];
    this.search = "";
    this.loading = false;
    this.modalError = null;
    this.btnStatus = false;
  }

  onSubmit() {
    this.loading = true;
    this.query.searchBooks(this.search)
      .subscribe(
        (data) => {
          this.loading = false;
          this.books = data;
          console.log(this.books);
        },
        (error) => {
          this.loading = false;
          this.errors.push("Failed to retrieve data from api.");
        }
      );
  }

  checkOutModel(template: TemplateRef<any>, bookData: any) {
    this.selectedBook = bookData;
    this.loan = new BookLoan();
    this.loan.Isbn = bookData[0]['book']['isbn'];
    this.btnStatus = true;
    this.modalError = null;
    this.modalRef = this.modalService.show(template);
  }

  checkOutBook() {
    this.query.checkOut(this.loan).subscribe(
      (data) => {
        if (data["error"]) {
          this.modalError = data["error"];
          this.btnStatus = false;
        }
        else {
          this.modalError = null;
          this.books[this.books.indexOf(this.selectedBook)].forEach(element => {
            element["book"]["isCheckedOut"] = true;
            this.modalRef.hide();
          });
        }
      },
      (error) => {
        this.modalError = "Failed to check the book out.";
        this.btnStatus = false;
      }
    );
  }

  removeError(err: string) {
    this.errors.splice(this.errors.indexOf(err), 1);
  }
}
