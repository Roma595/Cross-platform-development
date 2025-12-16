import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Profile } from '../interfaces/profile.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TeacherService {
  http = inject(HttpClient);

  baseApiUrl = 'http://localhost:5158/api/';
  
  getAllTeachers(): Observable<Profile[]> {
    return this.http.get<Profile[]>(`${this.baseApiUrl}Teacher`);
  }

  getTeacherById(id: number):Observable<Profile>{
    return this.http.get<Profile>(`${this.baseApiUrl}Teacher/${id}`);
  }

  createTeacher(teacher: Profile): Observable<Profile>{
    return this.http.post<Profile>(`${this.baseApiUrl}Teacher`, teacher);
  }

  updateTeacher(id: number, teacher: Profile) {
    return this.http.put<Profile>(`${this.baseApiUrl}Teacher/${id}`, teacher);
  }

  deleteTeacher(id: number): Observable<Profile>{
    return this.http.delete<Profile>(`${this.baseApiUrl}Teacher/${id}`);
  }
}
