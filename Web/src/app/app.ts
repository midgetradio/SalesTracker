import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { EditionsService } from '../services/editions.service';
import { Edition } from '../models/edition.model';
import { SaleType } from '../models/saletype.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  public http = inject(EditionsService);
  public editions = signal<Edition[]>([]);
  public saleTypes = signal<SaleType[]>([]);
  public dates = signal<string[]>([]);
  public selectedSaleTypes = signal<number[]>([]);
  public selectedDate = "";
  public filteredEditions = signal<Edition[]>([]);
  
  ngOnInit() {
    this.fetchEditions();
    this.fetchSaleTypes();
    this.fetchDates();
  }

  setSelectedSaleTypes(saleTypeId: number, isChecked: boolean): void {
    const currentSelected = this.selectedSaleTypes();
    if (isChecked) {
      this.selectedSaleTypes.set([...currentSelected, saleTypeId]);
    } else {
      this.selectedSaleTypes.set(currentSelected.filter(id => id !== saleTypeId));
    }
    this.filterEditionsBySelectedSaleTypes();
  }

  filterEditionsBySelectedSaleTypes(): void {
    const filteredEditions = this.editions().filter(edition => this.selectedSaleTypes().includes(edition.saleType.id));
    this.filteredEditions.set(filteredEditions);
  }

  filterEditionsByDate(date: string): void {
    const filteredEditions = this.filteredEditions().filter(edition => edition.lastUpdated.startsWith(date));
    this.filteredEditions.set(filteredEditions);
  }

  resetFilters(): void {
    this.fetchEditions();
  }

  sortEditionsByTitle(): void {
    const sortedEditions = [...this.filteredEditions()].sort((a, b) => {
      const titleA = a.title.toLowerCase();
      const titleB = b.title.toLowerCase();
      if (titleA < titleB) return -1;
      if (titleA > titleB) return 1;
      return 0;
    });
    this.filteredEditions.set(sortedEditions);
  } 

  sortEditionsByPrice(): void {
    const sortedEditions = [...this.filteredEditions()].sort((a, b) => {
      const priceA = parseFloat(a.price.replace(/[^0-9.-]+/g, ""));
      const priceB = parseFloat(b.price.replace(/[^0-9.-]+/g, ""));
      return priceA - priceB;
    });
    this.filteredEditions.set(sortedEditions);
  }

  sortEditionsByDiscount(): void {
    const sortedEditions = [...this.filteredEditions()].sort((a, b) => {
      const discountA = parseFloat(a.discount.replace(/[^0-9.-]+/g, ""));
      const discountB = parseFloat(b.discount.replace(/[^0-9.-]+/g, ""));
      return discountA - discountB;
    });
    this.filteredEditions.set(sortedEditions);
  }

  sortEditionsByLastUpdated(): void {
    const sortedEditions = [...this.filteredEditions()].sort((a, b) => {
      const dateA = new Date(a.lastUpdated);
      const dateB = new Date(b.lastUpdated);
      return dateA.getTime() - dateB.getTime();
    });
    this.filteredEditions.set(sortedEditions);
  }

  sortEditionsBySaleType(): void {
    const sortedEditions = [...this.filteredEditions()].sort((a, b) => {
      const saleTypeA = a.saleType.type.toLowerCase();
      const saleTypeB = b.saleType.type.toLowerCase();
      if (saleTypeA < saleTypeB) return -1;
      if (saleTypeA > saleTypeB) return 1;
      return 0;
    });
    this.filteredEditions.set(sortedEditions);
  }

  private fetchDates(): void {
    this.http.getDates().subscribe({
      next: (data) => {
        this.dates.set(data);
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  private fetchEditions(): void {
    this.http.getEditions().subscribe({
      next: (data) => {
        this.editions.set(data);
        this.filteredEditions.set(data);
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  public fetchEditionsByDate(): void {
    if(!this.selectedDate || this.selectedDate === "All Time")
    {
      this.fetchEditions();
      return;
    }

    let formattedDate = new Date(this.selectedDate).toISOString().split('T')[0];
    this.http.getEditionsByDate(formattedDate).subscribe({
      next: (data) => {
        this.editions.set(data);
        this.filteredEditions.set(data);
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  private fetchSaleTypes(): void {
    this.http.getSaleTypes().subscribe({
      next: (data) => {
        this.saleTypes.set(data);
        this.selectedSaleTypes.set(data.map(saleType => saleType.id));
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
