import { Component, Input } from '@angular/core';
import { Course } from '../../data/interfaces/course.interface';

@Component({
  selector: 'app-course-card',
  imports: [],
  templateUrl: './course-card.html',
  styleUrl: './course-card.scss',
})
export class CourseCard {
  @Input() course!: Course
}
