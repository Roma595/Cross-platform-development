import { Component, inject, OnInit, signal } from '@angular/core';
import { StudentService } from '../../data/services/student.service';
import { Profile } from '../../data/interfaces/profile.interface';
import { ProfileCard } from "../../common-ui/profile-card/profile-card";
import { CourseService } from '../../data/services/course.service';
import { Course } from '../../data/interfaces/course.interface';
import { CourseCard } from '../../common-ui/course-card/course-card';

@Component({
  selector: 'app-courses-page',
  imports: [CourseCard],
  templateUrl: './courses-page.html',
  styleUrl: './courses-page.scss',
})
export class CoursesPage implements OnInit {
  private courseService = inject(CourseService);
  

  courses = signal<Course[]>([]);
  loading = signal(true);

  ngOnInit(): void {
    this.loadStudents();
  }

  private loadStudents(): void {
    this.courseService.getAllCourses().subscribe({
      next: (data: Course[]) => {
        this.courses.set(data);  
        this.loading.set(false);
      },
      error: err => {
        this.loading.set(false);
      }
    });
  }
}
