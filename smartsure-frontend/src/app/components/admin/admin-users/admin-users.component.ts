import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-users.component.html'
})
export class AdminUsersComponent implements OnInit {
  users: any[] = [];
  loading = true;
  selectedUser: any = null;
  updating = false;
  error = '';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.http.get<any[]>(`${environment.apiUrl}/users`).subscribe({
      next: u => { this.users = u; this.loading = false; },
      error: () => this.loading = false
    });
  }

  openEdit(user: any) {
    this.selectedUser = { ...user };
    this.error = '';
  }

  closeModal() { this.selectedUser = null; }

  updateUser() {
    this.updating = true;
    this.http.put(`${environment.apiUrl}/users/${this.selectedUser.id}`, {
      fullName: this.selectedUser.fullName,
      role: this.selectedUser.role
    }).subscribe({
      next: (updated: any) => {
        const idx = this.users.findIndex(u => u.id === updated.id);
        if (idx !== -1) this.users[idx] = updated;
        this.updating = false;
        this.closeModal();
      },
      error: err => { this.error = err.error?.message || 'Update failed'; this.updating = false; }
    });
  }

  deleteUser(id: string) {
    if (!confirm('Are you sure you want to delete this user?')) return;
    this.http.delete(`${environment.apiUrl}/users/${id}`).subscribe({
      next: () => this.users = this.users.filter(u => u.id !== id)
    });
  }
}
