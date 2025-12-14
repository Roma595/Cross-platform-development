import { Component, EventEmitter, inject, Input, Output, signal } from '@angular/core';
import { Course } from '../../data/interfaces/course.interface';
import { CardModule } from 'primeng/card';
import { Profile } from '../../data/interfaces/profile.interface';
import { TeacherService } from '../../data/services/teacher.service';
import { CourseService } from '../../data/services/course.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-course-card',
  imports: [CardModule],
  templateUrl: './course-card.html',
  styleUrl: './course-card.scss',
})
export class CourseCard {
  course_service = inject(CourseService);
  cookieService = inject(CookieService);
  teacher_service = inject(TeacherService);

  @Output() edit = new EventEmitter<void>();
  @Output() delete = new EventEmitter<void>();
  @Input() course!: Course

  teacher = signal<Profile | null>(null);

  ngOnInit(){
    this.teacher_service.getTeacherById(this.course.teacherId).subscribe(teacher => {
      this.teacher.set(teacher);
    })
  }

}
