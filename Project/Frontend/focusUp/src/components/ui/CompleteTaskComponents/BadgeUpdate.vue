<script lang="ts" setup>
import PopUpTaskCompletion from '@/components/ui/CompleteTaskComponents/PopUpTaskCompletion.vue'

const props = defineProps<{
  badgeName: string
  badgeDescription: string
  svgColor: String
  backgroundBadgeColor: String
  badgeIcon: String
}>()

const emit = defineEmits<{
  (e: 'submit'): void
}>()

function onSubmit() {
  emit('submit')
}
</script>

<template>
  <PopUpTaskCompletion>
    <div class="shake base-element min-w-[500px]">
      <div class="flex flex-col justify-center items-center">
        <!-- Badge text-->
        <div class="appear w-full flex justify-center items-center gap-2 text-sm">
          <i class="fa-solid fa-trophy"></i>
          <span class="uppercase">Badge freigeschaltet!</span>
        </div>

        <!-- Badge icon-->
        <div
          class="stamp z-1 relative w-[100px] h-[100px] flex justify-center items-center text-[50px] bg-orange-100 border border-b-orange-500 text-orange-500 rounded-full mt-5"
        >
          <div class="absolute w-full h-full ping bg-orange-100 rounded-full z-0"></div>
          <i class="fa-solid fa-fire z-20"></i>
        </div>

        <div
          class="flex flex-col justify-center items-center gap-2 text-sm text-center mt-3 max-w-[400px]"
        >
          <span style="--delay: 0.3s" class="appear text-xl font-semibold">{{ props.badgeName }}</span>
          <p class="appear" style="--delay: 0.4s">
            {{ props.badgeDescription }}
          </p>
        </div>

        <!-- divider-->
        <div class="appear w-full h-0.5 bg-gray-100 my-3"></div>

        <!-- Continue button-->
        <div
          @click="onSubmit"
          class="appear scale-animation-sm cursor-pointer rounded-lg w-full px-5 py-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] flex justify-center items-center"
        >
          <button class="cursor-pointer" style="--delay: 0.6s">Fertig</button>
        </div>
      </div>
    </div>
  </PopUpTaskCompletion>
</template>

<style scoped>
.ping {
  animation: ping 1.5s infinite ease-out;
}

@keyframes ping {
  0% {
    transform: scale(1);
    opacity: 1;
  }
  70% {
    transform: scale(1.5);
    opacity: 0;
  }
  100% {
    transform: scale(1.5);
    opacity: 0;
  }
}

.appear {
  animation: appear 0.5s ease-out forwards;
  animation-delay: var(--delay);
  opacity: 0;
}

@keyframes appear {
  0% {
    opacity: 0;
  }
  70% {
    opacity: 1;
  }
  100% {
    opacity: 1;
  }
}

.stamp {
  animation: stamp 0.6s ease-in forwards;
}

.stamp {
  animation: stamp 0.6s ease-in forwards;
}

@keyframes stamp {
  0% {
    opacity: 0;
    transform: scale(1.5);
  }
  50% {
    opacity: 1;
    transform: scale(1.4);
  }
  100% {
    opacity: 1;
    transform: scale(1);
  }
}

.shake {
  animation: shake 0.3s ease-in forwards;
  animation-delay: 0.62s;
}

@keyframes shake {
  0% {
    transform: scale(1) rotate(0deg);
  }
  20% {
    transform: scale(1.05) rotate(2deg);
  }
  40% {
    transform: scale(1) rotate(-2deg);
  }
  60% {
    transform: scale(1.05) rotate(2deg);
  }
  80% {
    transform: scale(1) rotate(-2deg);
  }
  100% {
    transform: scale(1) rotate(0deg);
  }
}
</style>
