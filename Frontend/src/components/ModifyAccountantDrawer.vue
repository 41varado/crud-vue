<template>
  <Toast />
  <Dialog v-model:visible="dialogVisible" modal header="Modify Accountant" @onHide="handleDialogClose" :style="{ width: '25rem' }">
    <div v-if="props.accountant">
      <div class="flex items-center gap-4 mb-4">
        <label for="name" class="font-semibold w-24">Name</label>
        <InputText v-model="accountantToLoad!.name" required id="name" class="flex-auto" autocomplete="off"></InputText>
      </div>
      <div class="flex items-center gap-4 mb-4">
        <label for="birthDate" class="font-semibold w-24">Birth date</label>
        <DatePicker v-model="correctBirthDate" name="birthDate" fluid />
      </div>
      <div class="flex justify-end gap-4">
        <Button label="Save" @click="saveAccountant" />
      </div>
    </div>
  </Dialog>
</template>

<script setup lang="ts">
import type { Accounting } from '@/models/Accounting';
import { onMounted, ref, toRaw, watch } from 'vue';
import axios from 'axios';
import Toast from 'primevue/toast';
import { useToast } from 'primevue/usetoast';
import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import DatePicker from 'primevue/datepicker';
import InputText from 'primevue/inputtext';

const props = defineProps<{
  accountant: Accounting | null;
}>();
const accountantToLoad = ref<Accounting | null>(null);
const dialogVisible = ref<boolean>(props.accountant !== null);
const correctBirthDate = ref<Date | null>(null);
const toast = useToast();


const parseDate = (dateString: string): Date | null => {
  const parts = dateString.split('/');
  if (parts.length !== 3) return null; 
  const day = parseInt(parts[0], 10);
  const month = parseInt(parts[1], 10) - 1;
  const year = parseInt(parts[2], 10);

  const parsedDate = new Date(year, month, day);
  return isNaN(parsedDate.getTime()) ? null : parsedDate;
};

const emit = defineEmits(['accountantListModified', 'close']);

watch(
  () => props.accountant,
  (newAccountant) => {
    if (newAccountant) {
      accountantToLoad.value = structuredClone(newAccountant);
      dialogVisible.value = true;
    } else {
      dialogVisible.value = false;
    }
  }
);

const handleDialogClose = () => {
  accountantToLoad.value = null;
  correctBirthDate.value = null;
  dialogVisible.value = false;
  emit('close');
};

const saveAccountant = () => {
  if (accountantToLoad.value?.name && correctBirthDate) {
    const name = accountantToLoad.value.name;

    axios.patch(`https://localhost:44349/api/v1/accounting/${name}`, {
      name: accountantToLoad.value.name,
      birthDate: correctBirthDate.value!.toISOString()
    })
      .then((response) => {
        console.log(response);
        toast.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Accountant modified successfully!',
          life: 3000
        });
        emit('accountantListModified');
      })
      .catch((error) => {
        console.error('Error saving accountant:', error);
        toast.add({
          severity: 'error',
          summary: 'Error',
          detail: error.message,
          life: 3000
        });
      })
  } else {
    toast.add({
      severity: 'error',
      summary: 'Error',
      detail: 'Please fill in all fields.',
      life: 3000
    })
  }
  handleDialogClose();
}

onMounted(() => {
  correctBirthDate.value = parseDate(props.accountant!.birthDate);
})

if (props.accountant) {
    accountantToLoad.value = { ...props.accountant };
}
</script>