import { Component, inject, signal } from '@angular/core';
import { StudentService } from '../../data/services/student.service';
import { Profile } from '../../data/interfaces/profile.interface';
import { ProfileCard } from '../../common-ui/profile-card/profile-card';

@Component({
  selector: 'app-students-page',
  imports: [ProfileCard],
  templateUrl: './students-page.html',
  styleUrl: './students-page.scss',
})
export class StudentsPage {
  private studentService = inject(StudentService);
  

  students = signal<Profile[]>([]);
  loading = signal(true);

  ngOnInit(): void {
    this.loadStudents();
  }

  private loadStudents(): void {
    this.studentService.getAllStudents().subscribe({
      next: (data: Profile[]) => {
        this.students.set(data);  
        this.loading.set(false);
      },
      error: err => {
        this.loading.set(false);
      }
    });
  }
}
