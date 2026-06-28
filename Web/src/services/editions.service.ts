import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Edition } from '../models/edition.model';
import { Observable } from 'rxjs/internal/Observable';
import { SaleType } from '../models/saletype.model';

@Injectable({
  providedIn: 'root',
})
export class EditionsService {
  private http = inject(HttpClient);

  public getEditions(): Observable<Edition[]> {
    return this.http.get<Edition[]>('http://127.0.0.1:5103/editions');
  }

  public getEditionsByDate(date: string): Observable<Edition[]> {
    return this.http.get<Edition[]>(`http://127.0.0.1:5103/editions/${date}`);
  }

  public getSaleTypes(): Observable<SaleType[]> {
    return this.http.get<SaleType[]>('http://127.0.0.1:5103/saletypes');
  }

  public getDates(): Observable<string[]> {
    return this.http.get<string[]>('http://127.0.0.1:5103/editions/dates');
  }
}
