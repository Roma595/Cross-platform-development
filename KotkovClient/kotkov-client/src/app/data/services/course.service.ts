import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Course, CourseShort } from '../interfaces/course.interface';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  http = inject(HttpClient);

  baseApiUrl = 'http://localhost:5158/api/';
  
  getAllCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(`${this.baseApiUrl}Course`);
  }

  createCourse(course: Course): Observable<Course>{
    return this.http.post<Course>(`${this.baseApiUrl}Course`, course);
  }

  deleteCourse(id: number): Observable<Course>{
    return this.http.delete<Course>(`${this.baseApiUrl}Course/${id}`);
  }

  updateCourse(id: number, course: Course) {
    return this.http.put<Course>(`${this.baseApiUrl}Course/${id}`, course);
  }

  getAllCoursesForStudent(studentId: number) {
    return this.http.get<Course[]>(`${this.baseApiUrl}Course/${studentId}/courses`);
  }
}
