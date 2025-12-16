import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Profile } from '../interfaces/profile.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  http = inject(HttpClient);

  baseApiUrl = 'http://localhost:5158/api/';
  
  getAllStudents(): Observable<Profile[]> {
    return this.http.get<Profile[]>(`${this.baseApiUrl}Student`);
  }

  getStudentById(studentId: number): Observable<Profile> {
    return this.http.get<Profile>(`${this.baseApiUrl}Student/${studentId}`);
  }

  createStudent(student: Profile): Observable<Profile>{
    return this.http.post<Profile>(`${this.baseApiUrl}Student`, student);
  }

  updateStudent(id: number, student: Profile) {
    return this.http.put<Profile>(`${this.baseApiUrl}Student/${id}`, student);
  }

  deleteStudent(id: number): Observable<Profile>{
    return this.http.delete<Profile>(`${this.baseApiUrl}Student/${id}`);
  }

  addToCourse(studentId: number, courseId: number, statusId: number): Observable<any>{
    return this.http.post(`${this.baseApiUrl}Student/add_to_course`, {
      studentId,
      courseId,
      statusId
    });
  }

  getAllStudentsByCourseId(courseId: number): Observable<any>{
    return this.http.get<Profile>(`${this.baseApiUrl}Student/${courseId}/students`);
  }
}
