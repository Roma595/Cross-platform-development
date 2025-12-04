import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  http = inject(HttpClient);
  
  getAllStudents() {
    return this.http.get('/api/students');
  }
}
