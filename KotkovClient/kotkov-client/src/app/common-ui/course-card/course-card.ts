import { Component, EventEmitter, inject, Input, Output, signal } from '@angular/core';
import { Course } from '../../data/interfaces/course.interface';
import { CardModule } from 'primeng/card';
import { Profile } from '../../data/interfaces/profile.interface';
import { TeacherService } from '../../data/services/teacher.service';
import { CookieService } from 'ngx-cookie-service';
import { StudentService } from '../../data/services/student.service';

@Component({
  selector: 'app-course-card',
  imports: [CardModule],
  templateUrl: './course-card.html',
  styleUrl: './course-card.scss',
})
export class CourseCard {
  cookieService = inject(CookieService);
  teacherService = inject(TeacherService);
  studentService = inject(StudentService);

  @Output() edit = new EventEmitter<void>();
  @Output() delete = new EventEmitter<void>();
  @Input() course!: Course

  teacher = signal<Profile | null>(null);
  takenPlaces = signal<number>(0);

  ngOnInit(){
    this.teacherService.getTeacherById(this.course.teacherId).subscribe(teacher => {
      this.teacher.set(teacher);
    })

    this.studentService.getAllStudentsByCourseId(this.course.id).subscribe({
      next: students =>{
        this.takenPlaces.set(students.length)
      },
      error: () =>{
        console.log("error takenPlaces course");
        this.takenPlaces.set(0);
      }
    })
  }
}
