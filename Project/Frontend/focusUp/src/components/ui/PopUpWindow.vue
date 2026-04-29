<script lang="ts" setup>
  const emit = defineEmits<{
    (e: 'cancel') : void
    (e: 'submit') : void
  }>();

  const props = defineProps<{
    title: string,
  }>()

  function cancel() {
    emit('cancel');
  }
</script>

<template>
    <div class="absolute w-[100vw] h-[100vh] flex justify-center items-center backdrop-blur-xs z-999">
      <div class="base-element">
        <!-- Title and close-->
        <div class="flex items-center justify-between min-w-[500px]">
          <span class="font-semibold text-xl">{{ props.title }}</span>
          <button @click="cancel" class="hover:border-[var(--primary-color)] hover:text-[var(--primary-color)] duration-200 transition border-1 border-transparent flex justify-center items-center bg-gray-100 text-gray-400 w-[35px] h-[35px] rounded-lg text-sm cursor-pointer">
            <i class="fa-solid fa-x"></i>
          </button>
        </div>

        <slot></slot>
      </div>
    </div>
</template>

