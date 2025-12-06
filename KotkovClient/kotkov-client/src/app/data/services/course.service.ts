import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Course } from '../interfaces/course.interface';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  http = inject(HttpClient);

  baseApiUrl = 'http://localhost:5158/';
  
  getAllCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(`${this.baseApiUrl}api/Course`);
  }
}
