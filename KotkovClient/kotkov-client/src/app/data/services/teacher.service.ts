import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Profile } from '../interfaces/profile.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TeacherService {
  http = inject(HttpClient);

  baseApiUrl = 'http://localhost:5158/';
  
  getAllTeachers(): Observable<Profile[]> {
    return this.http.get<Profile[]>(`${this.baseApiUrl}api/Teacher`);
  }
}
