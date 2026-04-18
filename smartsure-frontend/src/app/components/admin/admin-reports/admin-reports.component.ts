import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-admin-reports',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-reports.component.html'
})
export class AdminReportsComponent implements OnInit {
  reports: any[] = [];
  loading = true;
  generating = false;
  reportType = 'Claims';
  error = '';
  success = '';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadReports();
  }

  loadReports() {
    this.http.get<any[]>(`${environment.apiUrl}/admin/reports`).subscribe({
      next: r => { this.reports = r; this.loading = false; },
      error: () => this.loading = false
    });
  }

  generateReport() {
    this.generating = true;
    this.error = '';
    this.http.post<any>(`${environment.apiUrl}/admin/reports/generate`, { reportType: this.reportType }).subscribe({
      next: report => {
        this.reports.unshift(report);
        this.success = `${this.reportType} report generated!`;
        this.generating = false;
        setTimeout(() => this.success = '', 3000);
      },
      error: err => { this.error = err.error?.message || 'Failed'; this.generating = false; }
    });
  }
}
