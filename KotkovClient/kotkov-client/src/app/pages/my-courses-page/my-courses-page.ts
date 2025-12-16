import { Component, inject, signal } from '@angular/core';
import { CourseService } from '../../data/services/course.service';
import { CookieService } from 'ngx-cookie-service';
import { TeacherService } from '../../data/services/teacher.service';
import { StudentService } from '../../data/services/student.service';
import { Course, CoursesViewMode } from '../../data/interfaces/course.interface';
import { CourseCard } from '../../common-ui/course-card/course-card';
import { Profile } from '../../data/interfaces/profile.interface';

@Component({
  selector: 'app-my-courses-page',
  imports: [CourseCard],
  templateUrl: './my-courses-page.html',
  styleUrl: './my-courses-page.scss',
})
export class MyCoursesPage {
  courseService = inject(CourseService);
  cookieService = inject(CookieService);
  teacherService = inject(TeacherService);
  studentService = inject(StudentService);

  courses = signal<Course[]>([]);
  teachers = signal<Profile[]>([]);
  allcourses = signal<Course[]>([]);

  teacherSearch = '';

  countTakePlaces = signal<number>(0);
  searchQuery = '';

  ngOnInit(): void {
    if(this.cookieService.get('role') === 'Student'){
      this.loadCoursesForStudent();
    }
    else if (this.cookieService.get('role') === 'Teacher'){
      this.loadCoursesForTeacher();
    }
    
  }

  loadCoursesForStudent(): void {
    this.courseService.getAllCoursesForStudent(parseInt(this.cookieService.get('user_id'))).subscribe({
      next: courses =>{
        this.courses.set(courses);
      },
      error: ()=>{
        console.log("error load courses");
      }
    })
  }

  loadCoursesForTeacher(){
    this.courseService.getAllCourses().subscribe({
      next: courses =>{
          const onlyWithTeacher = courses.filter(c => c.teacherId == parseInt(this.cookieService.get('user_id')));
          this.courses.set(onlyWithTeacher);
      },
      error: () =>{
        console.log('error get all courses');
      }
    });
  }


  getTeacherName(teacherId: number){
    this.teacherService.getTeacherById(teacherId).subscribe({
      next: teacher =>{
        return `${teacher.firstName}${teacher.lastName}`;
      },
      error: ()=>{
        console.log('error getteacherName');
      }
    });
    return "";
  }

  formatDate(date: string | Date): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toLocaleDateString('ru-RU');
  }

  getFilteredCourses(): Course[] {
    if (!this.searchQuery.trim()) {
      return this.courses();
    }

    const query = this.searchQuery.toLowerCase().trim();

    return this.courses().filter(course => {
      const courseName = course.name.toLowerCase();
      const teacherName = (this.getTeacherName(course.teacherId)).toLowerCase();
      
      return courseName.includes(query) || teacherName.includes(query);
    });
  }

  onSearchChange(value: string) {
    this.searchQuery = value;
  }

  clearSearch() {
    this.searchQuery = '';
  }
}
