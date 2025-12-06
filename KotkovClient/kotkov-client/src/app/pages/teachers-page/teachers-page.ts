import { Component, inject, signal } from '@angular/core';
import { Profile } from '../../data/interfaces/profile.interface';
import { TeacherService } from '../../data/services/teacher.service';
import { ProfileCard } from '../../common-ui/profile-card/profile-card';

@Component({
  selector: 'app-teachers-page',
  imports: [ProfileCard],
  templateUrl: './teachers-page.html',
  styleUrl: './teachers-page.scss',
})
export class TeachersPage {
  private teacherService = inject(TeacherService);
  

  teachers = signal<Profile[]>([]);
  loading = signal(true);

  ngOnInit(): void {
    this.loadTeachers();
  }

  private loadTeachers(): void {
    this.teacherService.getAllTeachers().subscribe({
      next: (data: Profile[]) => {
        this.teachers.set(data);  
        this.loading.set(false);
      },
      error: err => {
        this.loading.set(false);
      }
    });
  }
}
